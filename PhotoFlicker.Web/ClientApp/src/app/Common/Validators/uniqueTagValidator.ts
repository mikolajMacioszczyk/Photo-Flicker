import {AbstractControl, AsyncValidatorFn, ValidationErrors} from "@angular/forms";
import {TagService} from "../../Services/tag.service";
import {Observable, of} from "rxjs";
import {catchError, map} from "rxjs/operators";

export function uniqueTagName(service: TagService): AsyncValidatorFn {
  return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> => {
    return service.isTagUnique(control.value).pipe(
      map(isUnique => isUnique ? null : {isUnique: false}),
      catchError(e => of(e))
    );
  };
}
