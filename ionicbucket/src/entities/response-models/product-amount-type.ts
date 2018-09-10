import { UserLow } from "./user-low";

export class ProductAmountType{
    id: number;
    amountType: number;
    price: number;
    weight: number;
    createUser: UserLow;
    updateUser: UserLow;
    stock:number;
}