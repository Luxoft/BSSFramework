import { Pipe, PipeTransform } from '@angular/core';
import { IEntity } from '../rights/components/principals/components/view-principal-dialog/view-principal-dialog.component';
import { FormControl } from '@angular/forms';

@Pipe({
  name: 'filterContexts',
  pure: false,
  standalone: true,
})
export class FilterContextsPipe implements PipeTransform {
  transform(entities: FormControl<IEntity | null>[], search: string): FormControl<IEntity | null>[] {
    return search ? entities.filter((e) => e.value?.Name.toLocaleLowerCase().includes(search.toLowerCase())) : entities;
  }
}
