import { HttpClient } from '@angular/common/http';
import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { firstValueFrom, map, Observable } from 'rxjs';
import { HttpService } from '../services/http.service';

@Pipe({
  name: 'secure'
})
export class SecurePipe implements PipeTransform {
  constructor(private http: HttpClient, private sanitizer: DomSanitizer, private httpService: HttpService) {}
  transform(url: string): Observable<SafeUrl> {
    return this.http
        .get(url, { responseType: 'blob', headers: { Authorization: `Bearer ${this.httpService.Token}` } })
        .pipe(map(blob => this.sanitizer.bypassSecurityTrustUrl(URL.createObjectURL(blob))));
  }

}
