import {ITag} from "./Tag";

export interface IPhoto{
  id: number,
  path: string,
  tags: ITag[]
}
