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
import {MatRadioModule} from "@angular/material/radio";
import {NgxMatDatetimePickerModule, NgxMatTimepickerModule} from "@angular-material-components/datetime-picker";
import {MatDatepickerModule} from "@angular/material/datepicker";
import {MatNativeDateModule} from "@angular/material/core";
import {MatExpansionModule} from "@angular/material/expansion";
import {MatChipsModule} from "@angular/material/chips";
import {MatListModule} from "@angular/material/list";
import {ChartDialog} from "./chart-dialog/chart-dialog.component";
import {BarChartModule, LineChartModule, NgxChartsModule, TooltipModule} from "@swimlane/ngx-charts";
import {DatePipe} from "@angular/common";


@NgModule({
    imports: [
        BrowserModule,
        MatDatepickerModule,
        NgxMatTimepickerModule,
        NgxMatDatetimePickerModule,
        HttpClientModule,
        ReactiveFormsModule,
        LeafletModule,
        BrowserAnimationsModule,
        MatDialogModule,
        MatButtonModule,
        MatNativeDateModule,
        NgSelectModule,
        FormsModule,
        RouterModule.forRoot([
            {path: '', component: MainPageComponent},
        ]),
        MatInputModule,
        MatTableModule,
        MatAutocompleteModule,
        MatSelectModule,
        MatRadioModule,
        MatExpansionModule,
        MatChipsModule,
        TooltipModule,
        NgxChartsModule,
        MatListModule,
        LineChartModule
    ],
    declarations: [
        AppComponent,
        TopBarComponent,
        InfoPopupDialog,
        MainPageComponent,
        ChartDialog,
        LocationParameterDialog,
    ],
    providers: [DatePipe],
    bootstrap: [
        AppComponent
    ]
})
export class AppModule {
}
