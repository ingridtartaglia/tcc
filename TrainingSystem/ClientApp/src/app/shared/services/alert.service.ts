import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { Router, NavigationStart } from '@angular/router';
import { Observable } from 'rxjs/Observable';

export class Alert {
  type: string;
  message: string;
}

@Injectable()
export class AlertService {
  private subject = new Subject<Alert>();
  private keepAfterRouteChange = false;

  constructor(private router: Router) {
    // clear alert messages on route change unless 'keepAfterRouteChange' flag is true
    router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        if (this.keepAfterRouteChange) {
          // only keep for a single route change
          this.keepAfterRouteChange = false;
        } else {
          // clear alert messages
          this.clearAlert();
        }
      }
    });
  }

  getAlert(): Observable<any> {
    return this.subject.asObservable();
  }

  alert(type: string, message: string) {
    window.scroll(0, 0);
    this.subject.next(<Alert>{ type: type, message: message });
  }

  success(message: string) {
    this.alert('success', message);
  }

  error(res: any) {
    for (const key in res.error) {
      if (res.error.hasOwnProperty(key)) {
        const error = res.error[key];
        this.alert('danger', error[0]);
      }
    }
  }

  clearAlert() {
    this.subject.next();
  }
}
