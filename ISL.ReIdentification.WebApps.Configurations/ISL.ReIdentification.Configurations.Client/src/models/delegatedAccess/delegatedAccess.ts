import { Guid } from 'guid-typescript';

export class DelegatedAccess {
    public id: Guid;
    public requesterEmail: string;
    public recipientEmail: string;
    public isDelegatedAccess: boolean;
    public isApproved: boolean;
    public data: Uint8Array;
    public identifierColumn: string;
    public createdBy?: string;
    public createdDate?: Date | undefined;
    public updatedBy?: string;
    public updatedDate?: Date | undefined;

    constructor(delegatedAccess: any) {
        this.id = delegatedAccess.id ? Guid.parse(delegatedAccess.id) : Guid.parse(Guid.EMPTY);
        this.requesterEmail = delegatedAccess.requesterEmail || "";
        this.recipientEmail = delegatedAccess.recipientEmail || "";
        this.isDelegatedAccess = delegatedAccess.isDelegatedAccess || false;
        this.isApproved = delegatedAccess.isApproved || false;
        this.data = delegatedAccess.data || 0;
        this.identifierColumn = delegatedAccess.identifierColumn || ""; 
        this.createdDate = delegatedAccess.createdDate ? new Date(delegatedAccess.createdDate) : undefined;
        this.createdBy = delegatedAccess.createdBy || "";
        this.updatedDate = delegatedAccess.updatedDate ? new Date(delegatedAccess.updatedDate) : undefined;
        this.updatedBy = delegatedAccess.updatedBy || "";
    }
}