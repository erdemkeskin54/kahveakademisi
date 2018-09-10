import { ErrorContent } from "./error-content";

export class ApiResponse{
    isSucces:boolean;
    returnObject:any;
    errorContent:ErrorContent[];
}