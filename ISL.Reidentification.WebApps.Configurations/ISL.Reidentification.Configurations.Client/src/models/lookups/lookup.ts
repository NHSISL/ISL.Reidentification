import { Guid } from 'guid-typescript';

export class Lookup {
    public id: Guid;
    public name: string;
    public value: string;
    public createdBy?: string;
    public createdDate?: Date | undefined;
    public updatedBy?: string;
    public updatedDate?: Date | undefined;

    constructor(lookup: any) {
        this.id = lookup.id ? Guid.parse(lookup.id) : Guid.parse(Guid.EMPTY);
        this.name = lookup.name || "";
        this.value = lookup.value || "";
        this.createdDate = lookup.createdDate ? new Date(lookup.createdDate) : undefined;
        this.createdBy = lookup.createdBy || "";
        this.updatedDate = lookup.updatedDate ? new Date(lookup.updatedDate) : undefined;
        this.updatedBy = lookup.updatedBy || "";
    }
}