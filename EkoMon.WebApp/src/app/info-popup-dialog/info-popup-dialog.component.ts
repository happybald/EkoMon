import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";

@Component({
    templateUrl: './info-popup-dialog.component.html',
    styleUrls: ['./info-popup-dialog.component.css']
})
export class InfoPopupDialog implements OnInit {

    title?: string;

    constructor(    @Inject(MAT_DIALOG_DATA) public data: InfoPopupDialogModel) {
    }

    async ngOnInit(): Promise<void> {
        this.title = this.data.title;
        console.log(this.data.title);
    }

}

export interface InfoPopupDialogModel {
    title: string;
}