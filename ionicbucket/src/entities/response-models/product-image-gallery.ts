import { UserLow } from "./user-low";

export class ProductImageGallery{
    id:number;
    imageTitle: string;
    imageUrl: string;
    isActive: boolean;
    createDate: string;
    updateDate: string;
    createUser: UserLow;
    updateUser: UserLow;
}