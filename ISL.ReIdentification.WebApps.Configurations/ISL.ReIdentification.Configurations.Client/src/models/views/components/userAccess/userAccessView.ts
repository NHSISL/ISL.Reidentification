import { Guid } from 'guid-typescript';

export class UserAccessView {
    public id: Guid;
    public userEmail: string;
    public recipientEmail: string;
    public orgCode: string;
    public activeFrom: Date;
    public activeTo?: Date;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(
        id: Guid,
        userEmail: string,
        recipientEmail: string,
        orgCode: string,
        activeFrom: Date,
        activeTo?: Date,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,
    ) {
        this.id = id;
        this.userEmail = userEmail || "";
        this.recipientEmail = recipientEmail || "";
        this.orgCode = orgCode || "";
        this.activeFrom = activeFrom;
        this.activeTo = activeTo;
        this.createdBy = createdBy !== undefined ? createdBy : '';
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy !== undefined ? updatedBy : '';
        this.updatedDate = updatedDate;
    }
}