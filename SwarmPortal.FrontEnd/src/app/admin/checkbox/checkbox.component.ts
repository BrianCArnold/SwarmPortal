import { AfterViewInit, Component, ViewChild, ViewContainerRef } from '@angular/core';
import { ICellEditorAngularComp } from "ag-grid-angular";
import { ICellEditorParams } from 'ag-grid-community';

@Component({
  selector: 'editor-checkbox',
  templateUrl: './checkbox.component.html',
  styleUrls: ['./checkbox.component.scss']
})
export class CheckboxEditor implements ICellEditorAngularComp, AfterViewInit {
  params!: ICellEditorParams;
  value: boolean = false;

  @ViewChild('input', { read: ViewContainerRef })
  public input!: ViewContainerRef;

  constructor() { }
  getValue() {
    return this.value;
  }
  agInit(params: ICellEditorParams): void {
    console.log(params);
    this.params = params;
    //Since clicking on this to edit it implies changing the value,
    // we change the value as soon as editting is started.
    this.value = !<boolean>params.value;
  }
  ngAfterViewInit(): void {
    setTimeout(() => this.input.element.nativeElement.focus(), 10);
  }
  focusIn?(): void {
    //do nothing
  }
  focusOut?(): void {
    //do nothing
  }
  afterGuiAttached?(): void {
    //do nothing
  }

  ngOnInit(): void {
  }

}
