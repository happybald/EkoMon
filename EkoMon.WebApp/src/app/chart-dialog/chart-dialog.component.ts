import {ChangeDetectorRef, Component, Inject, OnInit, Query, ViewChild} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialog, MatDialogConfig, MatDialogRef} from "@angular/material/dialog";
import {ApiClientModule} from "../api.module";
import {HttpClient, HttpClientModule} from "@angular/common/http";
import {last, lastValueFrom} from "rxjs";
import {LocationParameterDialog} from "../location-parameter-dialog/location-parameter-dialog.component";
import {MatTable, MatTableDataSource} from "@angular/material/table";
import {DatePipe} from "@angular/common";

@Component({
    templateUrl: './chart-dialog.component.html',
    styleUrls: ['./chart-dialog.component.scss']
})
export class ChartDialog implements OnInit {

    chartData?: ChartData[];

    constructor(private dialogRef: MatDialogRef<ChartDialog>, @Inject(MAT_DIALOG_DATA) public data: ApiClientModule.LocationParameterModel[], private datePipe: DatePipe) {
    }

    async ngOnInit(): Promise<void> {
        this.chartData = new Array<ChartData>();
        let chartLine = {name: this.data[0].parameter.title, series: new Array<Series>()} as ChartData;
        this.data.map(i => chartLine.series.push({
            name: this.datePipe.transform(i.dateTime, 'dd.MM.yyyy'),
            value: i.value
        } as Series));
        this.chartData.push(chartLine);
        console.log(this.chartData)
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
