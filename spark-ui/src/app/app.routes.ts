import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home';
import { ComputerList } from './pages/computer-list/computer-list';
import { Cart } from './pages/cart/cart';
import { ComputerDetails } from './pages/computer-details/computer-details';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'computers', component: ComputerList },
  { path: 'cart', component: Cart },
  { path: 'computers/:id', component: ComputerDetails }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
