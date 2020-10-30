import {ITag} from "./Tag";

export interface IPhoto{
  id: number,
  path: string,
  description: string,
  tags: ITag[]
}
