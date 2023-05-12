import {Component, NgZone, OnInit} from '@angular/core';
import {
    latLng,
    MapOptions,
    tileLayer,
    Map,
    marker,
    MarkerOptions,
    tooltip,
    TooltipOptions,
    LeafletMouseEvent
} from "leaflet";
import {ApiClientModule} from "../api.module";
import {MatDialog, MatDialogConfig} from "@angular/material/dialog";
import {InfoPopupDialog, InfoPopupDialogModel} from "../info-popup-dialog/info-popup-dialog.component";

@Component({
    selector: 'app-main-page',
    templateUrl: './main-page.component.html',
    styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit {

    options: MapOptions = {
        layers: [
            tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {maxZoom: 18, attribution: '...'})
        ],
        zoom: 13,
        center: latLng(50.449071, 30.526081),
    };

    map: Map = null!;

    addMode: boolean = false;

    constructor(private client: ApiClientModule.ApiClient, private dialog: MatDialog, private zone: NgZone) {
    }


    ngOnInit(): void {
    }

    onMapReady(map: Map) {
        this.map = map;
        this.client.api().subscribe();
        let newMarker = marker([50.43501, 30.51093]);
        newMarker.bindTooltip(tooltip({content: "Lol"} as TooltipOptions));
        newMarker.addTo(map);
    }

    resize($event: UIEvent) {
        this.map.invalidateSize();
    }

    toggleAddMarker($event: MouseEvent) {
        $event.stopPropagation();
        $event.stopImmediatePropagation();
        $event.preventDefault();
        this.addMode = !this.addMode;
        return false;
    }

    mapClick($event: LeafletMouseEvent) {
        if (this.addMode) {
            const title = $event.latlng.toString();
            let newMarker = marker($event.latlng, {
                title: title,
            });
            newMarker.addTo(this.map);
            newMarker.on("click", e => this.markerClickEventHandler(e))
        }
        this.addMode = false;
    }

    private markerClickEventHandler($event: LeafletMouseEvent) {
        const title = $event.target.options.title;
        this.zone.run(() => {
            const dialogConfig = {
                data: {title: title} as InfoPopupDialogModel,
            } as MatDialogConfig;
            const dialogRef = this.dialog.open(InfoPopupDialog, dialogConfig);

            dialogRef.afterClosed().subscribe(
                data => console.log("Dialog output:", data)
            );
        });
    }
}
