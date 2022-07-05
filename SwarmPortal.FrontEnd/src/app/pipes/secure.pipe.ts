import { HttpClient } from '@angular/common/http';
import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { firstValueFrom, map, Observable } from 'rxjs';
import { HttpService } from '../services/http.service';

@Pipe({
  name: 'secure',

})
export class SecurePipe implements PipeTransform {
  constructor(private http: HttpClient, private sanitizer: DomSanitizer, private httpService: HttpService) {

  }
  transform(url: string): Observable<SafeUrl> {
    // return this.httpService.Icon.iconUriGet()
    // return this.http
    //     .get(url, { responseType: 'blob', headers: { Authorization: `Bearer ${this.httpService.Token}` } })
    //     .pipe(map(blob => this.sanitizer.bypassSecurityTrustUrl(URL.createObjectURL(blob))));
        // this.httpService.Admin
    return new Observable<string>((observer) => {
          // This is a tiny blank image
      observer.next('data:image/gif;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==');
      this.httpService.GetNoIconDefault().subscribe(res => observer.next(res));

      const reader = new FileReader();
      reader.onloadend = _ => {
        let res = reader.result;
        if (typeof(res) === 'string') {
          observer.next(res);
        }
      };


      let subscrip = this.http
      .get(url, { responseType: 'blob', headers: { Authorization: `Bearer ${this.httpService.Token}` } })
      .subscribe(response => reader.readAsDataURL(response));

      return {
          unsubscribe() {
            subscrip.unsubscribe();
          }
      };
    });
  }

}
