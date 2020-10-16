import {IPhoto} from "./Photo";

export interface ITag{
  id: number,
  name: string,
  markedPhotos: IPhoto[]
}
