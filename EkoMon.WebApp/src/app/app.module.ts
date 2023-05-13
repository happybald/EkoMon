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
import {MatInputModule} from "@angular/material/input";
import {MatTableModule} from "@angular/material/table";
import {LocationParameterDialog} from "./location-parameter-dialog/location-parameter-dialog.component";
import {MatAutocompleteModule} from "@angular/material/autocomplete";
import {MatSelectModule} from "@angular/material/select";
import {NgSelectModule} from "@ng-select/ng-select";


@NgModule({
    imports: [
        BrowserModule,
        HttpClientModule,
        ReactiveFormsModule,
        LeafletModule,
        BrowserAnimationsModule,
        MatDialogModule,
        MatButtonModule,
        NgSelectModule,
        FormsModule,
        RouterModule.forRoot([
            {path: '', component: MainPageComponent},
        ]),
        MatInputModule,
        MatTableModule,
        MatAutocompleteModule,
        MatSelectModule
    ],
    declarations: [
        AppComponent,
        TopBarComponent,
        InfoPopupDialog,
        MainPageComponent,
        LocationParameterDialog,
    ],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule {
}
