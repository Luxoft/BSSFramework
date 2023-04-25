import { Directive, ElementRef, Input, OnChanges, SecurityContext, SimpleChanges } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Directive({
  selector: '[appHighlight]',
  standalone: true,
})
export class HighlightDirective implements OnChanges {
  @Input('appHighlight') searchTerm!: string | null | undefined;
  @Input() text!: string | null;

  constructor(private el: ElementRef, private sanitizer: DomSanitizer) {}

  ngOnChanges(changes: SimpleChanges) {
    if (this.el?.nativeElement) {
      if ('searchTerm' in changes || 'caseSensitive' in changes) {
        if (!this.searchTerm) {
          const sanitzed = this.sanitizer.sanitize(SecurityContext.HTML, this.text) || '';
          (this.el.nativeElement as HTMLElement).innerHTML = sanitzed;
        } else {
          const regex = new RegExp(this.searchTerm, 'gi');
          const newText = (this.text || '').replace(regex, (match: string) => {
            return `<mark>${match}</mark>`;
          });
          const sanitzed = this.sanitizer.sanitize(SecurityContext.HTML, newText) || '';
          (this.el.nativeElement as HTMLElement).innerHTML = sanitzed;
        }
      }
    }
  }
}
