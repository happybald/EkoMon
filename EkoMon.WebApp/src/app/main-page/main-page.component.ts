import {Component, NgZone, OnInit} from '@angular/core';
import {latLng, MapOptions, tileLayer, Map, marker, LeafletMouseEvent, LatLng, Marker, LayerGroup, layerGroup, icon, Icon} from "leaflet";
import {ApiClientModule} from "../api.module";
import {MatDialog, MatDialogConfig, MatDialogRef} from "@angular/material/dialog";
import {InfoPopupDialog, InfoPopupDialogModel} from "../info-popup-dialog/info-popup-dialog.component";

@Component({
    selector: 'app-main-page',
    templateUrl: './main-page.component.html',
    styleUrls: ['./main-page.component.scss']
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
    markersGroup: LayerGroup = null!;
    addMode: boolean = false;

    constructor(private apiClient: ApiClientModule.ApiClient, private dialog: MatDialog, private zone: NgZone) {
    }


    ngOnInit(): void {
    }

    onMapReady(map: Map) {
        this.map = map;
        this.markersGroup = layerGroup().addTo(this.map);
        this.updateData();
    }

    updateData() {
        this.markersGroup.clearLayers();
        this.apiClient.getLocations().subscribe(locations => {
            for (const location of locations) {
                this.createNewMarker(location);
            }
        });
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
            this.createNewMarker($event.latlng);
        }
        this.addMode = false;
    }

    createNewMarker(baseOn: ApiClientModule.LocationShortModel | LatLng) {

        let newMarker: Marker = null!;

        if (baseOn instanceof ApiClientModule.LocationShortModel) {
            newMarker = marker([baseOn.latitude, baseOn.longitude], {
                title: baseOn.title,
                attribution: baseOn.id.toString(),
                icon: icon({
                    ...Icon.Default.prototype.options,
                    iconUrl: 'assets/marker-icon.png',
                    iconRetinaUrl: 'assets/marker-icon-2x.png',
                    shadowUrl: 'assets/marker-shadow.png'
                })
            });
        }
        if (baseOn instanceof LatLng) {
            newMarker = marker(baseOn, {
                title: "New marker",
                icon: icon({
                    ...Icon.Default.prototype.options,
                    iconUrl: 'assets/marker-icon.png',
                    iconRetinaUrl: 'assets/marker-icon-2x.png',
                    shadowUrl: 'assets/marker-shadow.png'
                })
            });
        }
        newMarker.bindTooltip(newMarker.options.title!);
        newMarker.addTo(this.markersGroup);
        newMarker.on("click", e => this.markerClickEventHandler(e))
        if (baseOn instanceof LatLng) {
            newMarker.getElement()?.click();
        }
    }


    markerClickEventHandler($event: LeafletMouseEvent) {
        const title = $event.target.options.title;
        const id: number | undefined = $event.target.options.attribution as number | undefined;
        this.zone.run(() => {
            const dialogConfig = {
                width: "1000px",
                closeOnNavigation: false,
                disableClose: true,
                data: {
                    id: id,
                    title: title,
                    latitude: $event.latlng.lat,
                    longitude: $event.latlng.lng,
                } as InfoPopupDialogModel,
            } as MatDialogConfig;
            const dialogRef = this.dialog.open(InfoPopupDialog, dialogConfig) as MatDialogRef<InfoPopupDialog, ApiClientModule.LocationModel>;

            dialogRef.afterClosed().subscribe(r => {
                this.updateData();
            });
        });
    }
}
