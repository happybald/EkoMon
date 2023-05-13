import {ChangeDetectorRef, Component, Inject, OnInit, Query, ViewChild} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialog, MatDialogConfig, MatDialogRef} from "@angular/material/dialog";
import {ApiClientModule} from "../api.module";
import {HttpClient, HttpClientModule} from "@angular/common/http";
import {last, lastValueFrom} from "rxjs";
import {LocationParameterDialog} from "../location-parameter-dialog/location-parameter-dialog.component";
import {MatTable, MatTableDataSource} from "@angular/material/table";

@Component({
    templateUrl: './info-popup-dialog.component.html',
    styleUrls: ['./info-popup-dialog.component.scss']
})
export class InfoPopupDialog implements OnInit {
    @ViewChild(MatTable) matTable!: MatTable<ApiClientModule.LocationParameterModel>;

    location: ApiClientModule.LocationModel = null!;
    displayColumns: string[] = ["parameter.title", "value", "parameter.unit.title", "actions"]

    constructor(private dialogRef: MatDialogRef<InfoPopupDialog>, @Inject(MAT_DIALOG_DATA) public data: InfoPopupDialogModel, private apiClient: ApiClientModule.ApiClient, private httpClient: HttpClient, private dialog: MatDialog) {
    }

    async ngOnInit(): Promise<void> {
        if (this.data.id) {
            this.location = await lastValueFrom(this.apiClient.getLocation(this.data.id));
        } else {
            this.location = {
                id: 0,
                latitude: this.data.latitude,
                longitude: this.data.longitude,
                locationParameters: new Array<ApiClientModule.LocationParameterModel>(),
                title: this.data.title,
                address: "",
            } as ApiClientModule.LocationModel;
        }
        if (!this.location.address) {
            const addressFetch = await lastValueFrom(this.httpClient.get<any>(`https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=${this.location.latitude}&lon=${this.location.longitude}&accept-language=ua`));
            this.location.address = `${addressFetch.address.road}`
            if (addressFetch.address.house_number) {
                this.location.address += ` ${addressFetch.address.house_number}`;
            }
        }
    }
    
    close() {
        this.dialogRef.close();
    }

    save() {
        if (this.location.id == 0) {
            this.apiClient.postLocation(this.location).subscribe(_ => {
                this.close();
            })
        } else {
            this.apiClient.updateLocation(this.location).subscribe(_ => {
                this.close();
            })
        }
    }

    remove() {
        if (this.location.id != 0) {
            this.apiClient.deleteLocation(this.location.id).subscribe(_ => {
                this.close();
            })
        }
    }

    openLocationParameter(element: ApiClientModule.LocationParameterModel) {
        const dialogConfig = {
            width: "800px",
            height: "600px",
            closeOnNavigation: false,
            disableClose: true,
            data: element,
        } as MatDialogConfig;
        let dialogRef = this.dialog.open(LocationParameterDialog, dialogConfig) as MatDialogRef<LocationParameterDialog, ApiClientModule.LocationParameterModel>;
        dialogRef.afterClosed().subscribe(e => {
            element.parameter = e!.parameter;
            element.value = e!.value;
        })
    }

    addLocationParameter() {
        const newLocationParameter = new ApiClientModule.LocationParameterModel();

        const dialogConfig = {
            width: "800px",
            height: "600px",
            closeOnNavigation: false,
            disableClose: true,
            data: newLocationParameter,
        } as MatDialogConfig;
        let dialogRef = this.dialog.open(LocationParameterDialog, dialogConfig) as MatDialogRef<LocationParameterDialog, ApiClientModule.LocationParameterModel>;
        dialogRef.afterClosed().subscribe(e => {
            if (e) {
                this.location.locationParameters.push(e);
                console.log(this.location)
                this.matTable?.renderRows();
            }
        })
    }
}

export interface InfoPopupDialogModel {
    id?: number;
    title: string;
    latitude: number;
    longitude: number;
}

export interface InfoPopupDialogResult {
    location: ApiClientModule.LocationModel;
    remove: boolean;
}