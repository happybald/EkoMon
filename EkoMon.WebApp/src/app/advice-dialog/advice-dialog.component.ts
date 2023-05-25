import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {DatePipe} from "@angular/common";
import {ApiClientModule} from "../api.module";

@Component({
    templateUrl: './advice-dialog.component.html',
    styleUrls: ['./advice-dialog.component.scss']
})
export class AdviceDialog implements OnInit {

    constructor(private dialogRef: MatDialogRef<AdviceDialog>, @Inject(MAT_DIALOG_DATA) public data: string) {
    }

    async ngOnInit(): Promise<void> {
    }

    close() {
        this.dialogRef.close();
    }


}

export interface ChartData {
    name: string
    series: Series[]
}

export interface Series {
    name: string
    value: number
}
