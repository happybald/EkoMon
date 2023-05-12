import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {RouterModule} from '@angular/router';
import {HttpClientModule} from '@angular/common/http';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';

import {AppComponent} from './app.component';
import {TopBarComponent} from './top-bar/top-bar.component';
import {LeafletModule} from '@asymmetrik/ngx-leaflet';
import {MainPageComponent} from "./main-page/main-page.component";
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatDialogModule} from "@angular/material/dialog";
import {MatButtonModule} from "@angular/material/button";
import {InfoPopupDialog} from "./info-popup-dialog/info-popup-dialog.component";


@NgModule({
    imports: [
        BrowserModule,
        HttpClientModule,
        ReactiveFormsModule,
        LeafletModule,
        BrowserAnimationsModule,
        MatDialogModule,
        MatButtonModule,
        FormsModule,
        RouterModule.forRoot([
            {path: '', component: MainPageComponent},
        ])
    ],
    declarations: [
        AppComponent,
        TopBarComponent,
        InfoPopupDialog,
        MainPageComponent,
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule {
}
