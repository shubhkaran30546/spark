import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home';
import { ComputerList } from './pages/computer-list/computer-list';
import { CartService } from './pages/cart/cart';
import { ComputerDetails } from './pages/computer-details/computer-details';
import { LoginComponent } from './pages/auth/login/login';
import { RegisterComponent } from './pages/auth/register/register';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'computers', component: ComputerList },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'cart', component: CartService },
  { path: 'computers/:id', component: ComputerDetails }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
