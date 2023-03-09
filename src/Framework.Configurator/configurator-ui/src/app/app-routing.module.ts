import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ConstantsComponent } from './constants/constants.component';
import { EventsComponent } from './events/events.component';
import { RightsComponent } from './rights/rights.component';

const routes: Routes = [
  { path: '', component: RightsComponent },
  { path: 'events', component: EventsComponent },
  { path: 'constants', component: ConstantsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
