import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ApiClientModule} from "../api.module";
import {lastValueFrom} from "rxjs";


@Component({
    templateUrl: './location-parameter-dialog.component.html',
    styleUrls: ['./location-parameter-dialog.component.scss']
})
export class LocationParameterDialog implements OnInit {

    locationParameterClone: ApiClientModule.LocationParameterModel = null!;
    parameters: ApiClientModule.ParameterModel[] = null!;
    units: ApiClientModule.UnitModel[] = null!;

    constructor(private dialogRef: MatDialogRef<LocationParameterDialog>, @Inject(MAT_DIALOG_DATA) public data: ApiClientModule.LocationParameterModel, private apiClient: ApiClientModule.ApiClient) {
    }

    async ngOnInit(): Promise<void> {
        this.parameters = await lastValueFrom(this.apiClient.getParameters());
        this.units = await lastValueFrom(this.apiClient.getUnits());
        this.locationParameterClone = this.data.clone();
    }

    save() {
        this.dialogRef.close(this.locationParameterClone);
    }

    addParameter(title: string): ApiClientModule.ParameterModel {
        const newParam = new ApiClientModule.ParameterModel();
        newParam.id = 0;
        newParam.title = title;
        return newParam;
    }
    addUnit(title: string): ApiClientModule.UnitModel {
        const newUnit = new ApiClientModule.UnitModel();
        newUnit.id = 0;
        newUnit.title = title;
        return newUnit;
    }

    cancel() {
        this.dialogRef.close();
    }

    delete() {
        this.apiClient.deleteLocationParameter(this.locationParameterClone.id).subscribe(_=>{
            this.dialogRef.close();
        })
    }
}