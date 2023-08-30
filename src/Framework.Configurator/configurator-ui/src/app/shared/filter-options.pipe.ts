import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filterOptions',
  standalone: true,
})
export class FilterOptionsPipe<T extends { Name: string }> implements PipeTransform {
  transform(list: T[] | undefined, search: string | T | undefined): T[] {
    return typeof search === 'string' && list
      ? list.filter((item) => item.Name.toLocaleLowerCase().includes(search.toLocaleLowerCase()))
      : list || [];
  }
}
