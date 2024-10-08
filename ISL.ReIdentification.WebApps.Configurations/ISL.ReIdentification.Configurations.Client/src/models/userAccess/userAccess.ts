/* eslint-disable @typescript-eslint/no-explicit-any */
import { Guid } from 'guid-typescript';

export class UserAccess {
    public id: Guid;
    public userEmail: string;
    public recipientEmail: string;
    public orgCode: string;
    public activeFrom?: Date;
    public activeTo?: Date;
    public createdBy?: string;
    public createdDate?: Date | undefined;
    public updatedBy?: string;
    public updatedDate?: Date | undefined;

    constructor(user: any) {
        this.id = user.id ? Guid.parse(user.id) : Guid.parse(Guid.EMPTY);
        this.userEmail = user.userEmail || "";
        this.recipientEmail = user.recipientEmail || "";
        this.orgCode = user.orgCode || "";
        this.activeFrom = user.activeFrom;
        this.activeTo = user.activeTo;
        this.createdDate = user.createdDate ? new Date(user.createdDate) : undefined;
        this.createdBy = user.createdBy || "";
        this.updatedDate = user.updatedDate ? new Date(user.updatedDate) : undefined;
        this.updatedBy = user.updatedBy || "";
    }
}