
export class Cart {
    productId: number = 0;
    quantity: number = 0;
    productAmountType: number = 0;

    constructor(productId?, quantity?, productAmountType?) {

        this.productId = productId;
        this.quantity = quantity;
        this.productAmountType = productAmountType;

    }
}