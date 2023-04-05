import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  { path: '', loadComponent: () => import('./rights/rights.component').then((component) => component.RightsComponent) },
  { path: 'events', loadComponent: () => import('./events/events.component').then((component) => component.EventsComponent) },
  { path: 'constants', loadComponent: () => import('./constants/constants.component').then((component) => component.ConstantsComponent) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
