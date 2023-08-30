import { Pipe, PipeTransform } from '@angular/core';
import { IRoleContext } from './grant-rights-dialog.models';

@Pipe({
  name: 'contextStringFilter',
  standalone: true,
})
export class ContextStringFilterPipe implements PipeTransform {
  transform(
    filter:
      | {
          contextId: string;
          search: string;
        }[]
      | undefined,
    context: IRoleContext
  ): string {
    return filter?.find((f) => f.contextId === context.Id)?.search || '';
  }
}
