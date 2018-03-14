import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PlatformRoutingModule } from './platform-routing.module';
import { PlatformComponent } from './platform.component';

@NgModule({
  imports: [
    CommonModule,
    PlatformRoutingModule
  ],
  declarations: [PlatformComponent]
})
export class PlatformModule { }
