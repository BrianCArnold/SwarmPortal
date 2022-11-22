import { Component } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular'
import { ICellRendererParams } from 'ag-grid-community';

@Component({
  selector: 'checkbox-renderer',
  templateUrl: './checkbox.component.html',
  styleUrls: ['./checkbox.component.scss']
})
export class CheckboxRenderer implements ICellRendererAngularComp {
  value!: boolean;

  constructor() { }
  refresh(params: ICellRendererParams): boolean {
    this.value = <boolean>params.value;
    return true;
  }
  agInit(params: ICellRendererParams): void {
    this.value = <boolean>params.value;
  }

  ngOnInit(): void {
  }

}
