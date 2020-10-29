import {AbstractControl, AsyncValidatorFn, ValidationErrors} from "@angular/forms";
import {Observable} from "rxjs";

function loadImage(url){
  return new Promise((resolve, reject) => {
    const image = new Image();
    image.addEventListener('load', () => {
      resolve(null)
    });
    image.addEventListener('error', () => {
      reject({isFounded: false})
    })
    image.src = url;
  })
}

export function photoUrlExistValidator(): AsyncValidatorFn {
  return (control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null>  => {
    return  loadImage(control.value).catch(err => {return {photoUrlExistValidator: false}});};
}
