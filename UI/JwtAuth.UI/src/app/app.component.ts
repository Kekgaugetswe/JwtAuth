import { Component } from '@angular/core';
// import {  RouterOutlet } from '@angular/router';
import { RouterModule } from '@angular/router';
import { NavbarComponent } from "./core/components/navbar/navbar.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'JwtAuth.UI';
}
