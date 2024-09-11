import { Guid } from 'guid-typescript';

export class LookupView {
    public id: Guid;
    public lookupType: string;
    public name: string;
    public value: string;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(
        id: Guid,
        lookupType?: string,
        name?: string,
        value?: string,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,
    ) {
        this.id = id;
        this.lookupType = lookupType || "";
        this.name = name || "";
        this.value = value || "";
        this.createdBy = createdBy !== undefined ? createdBy : '';
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy !== undefined ? updatedBy : '';
        this.updatedDate = updatedDate;
    }
}