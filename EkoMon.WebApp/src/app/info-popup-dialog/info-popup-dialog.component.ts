import {ChangeDetectorRef, Component, Inject, OnInit, Query, ViewChild} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialog, MatDialogConfig, MatDialogRef} from "@angular/material/dialog";
import {ApiClientModule} from "../api.module";
import {HttpClient, HttpClientModule} from "@angular/common/http";
import {last, lastValueFrom} from "rxjs";
import {LocationParameterDialog} from "../location-parameter-dialog/location-parameter-dialog.component";
import {MatTable, MatTableDataSource} from "@angular/material/table";
import {ChartDialog} from "../chart-dialog/chart-dialog.component";

@Component({
    templateUrl: './info-popup-dialog.component.html',
    styleUrls: ['./info-popup-dialog.component.scss']
})
export class InfoPopupDialog implements OnInit {

    location: ApiClientModule.LocationModel = null!;

    categories: ApiClientModule.CategoryModel[] = new Array<ApiClientModule.CategoryModel>();
    currentCategory?: ApiClientModule.CategoryModel;

    filteredLocationParameters = () => {
        if (this.currentCategory)
            return this.location.groupedLocationParameters.filter(c => c.parameter.categoryId == this.currentCategory?.id);
        return this.location.groupedLocationParameters;
    }

    constructor(private dialogRef: MatDialogRef<InfoPopupDialog>, @Inject(MAT_DIALOG_DATA) public data: InfoPopupDialogModel, private apiClient: ApiClientModule.ApiClient, private httpClient: HttpClient, private dialog: MatDialog) {
    }

    async ngOnInit(): Promise<void> {
        this.categories = await lastValueFrom(this.apiClient.getCategories());
        if (this.data.categoryId)
            this.currentCategory = this.categories.find(i => i.id == this.data.categoryId)

        if (this.data.id) {
            this.location = await lastValueFrom(this.apiClient.getLocation(this.data.id));
        } else {
            const newLocation = {
                id: 0,
                latitude: this.data.latitude,
                longitude: this.data.longitude,
                groupedLocationParameters: new Array<ApiClientModule.GroupedLocationParameterModel>(),
                title: this.data.title,
                address: "",
            } as ApiClientModule.LocationModel;

            this.location = await lastValueFrom(this.apiClient.upsertLocation(newLocation));

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
            this.apiClient.postLocation(this.location).subscribe(shortLocation => {
                this.location.id = shortLocation.id;
                this.apiClient.upsertLocation(this.location).subscribe(_ => {
                    this.close();
                })
            })
        } else {
            this.apiClient.upsertLocation(this.location).subscribe(_ => {
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
            if (e) {
                this.apiClient.upsertLocationParameter(e, this.location.id).subscribe(async e => {
                    this.location = await lastValueFrom(this.apiClient.getLocation(this.location.id));
                })
            }
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
                this.apiClient.upsertLocationParameter(e, this.location.id).subscribe(async e => {
                    this.location = await lastValueFrom(this.apiClient.getLocation(this.location.id));
                })
            }
        })
    }

    openChart(locationParameters: ApiClientModule.LocationParameterModel[]) {
        const dialogConfig = {
            closeOnNavigation: false,
            disableClose: true,
            data: locationParameters,
        } as MatDialogConfig;
        let dialogRef = this.dialog.open(ChartDialog, dialogConfig) as MatDialogRef<ChartDialog>;
    }
}

export interface GroupedLocationParameterModel {
    parameter: ApiClientModule.ParameterModel;
    locationParameters: ApiClientModule.LocationParameterModel[]
}

export interface InfoPopupDialogModel {
    id?: number;
    title: string;
    latitude: number;
    longitude: number;
    categoryId?: number;
}

export interface InfoPopupDialogResult {
    location: ApiClientModule.LocationModel;
    remove: boolean;
}