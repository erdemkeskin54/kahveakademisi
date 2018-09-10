import { ProductCategory } from "./product-category";
import { ProductImageGallery } from "./product-image-gallery";
import { ProductAmountType } from "./product-amount-type";

export class Product{
        id:number;
        productStatus:number;
        productName:string;
        mainImage:string;
        createDate:string;
        updateDate:string;
        discount:boolean;
        discountAmount:number;
        discountStartDate:string
        discountFinishDate:string
        shortDescription:string;
        longDescription:string;
        stock:string;

        productCategories:ProductCategory[]=null;
        productImageGalleries:ProductImageGallery[]=null;
        productAmountTypes:ProductAmountType[]=null;
}