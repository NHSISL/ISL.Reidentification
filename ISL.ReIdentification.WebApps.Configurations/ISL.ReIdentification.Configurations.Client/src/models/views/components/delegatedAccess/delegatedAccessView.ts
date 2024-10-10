import { Guid } from 'guid-typescript';

export class ImpersonationContextView {
    public id: Guid;
    public requesterEmail: string;
    public recipientEmail: string;
    public isImpersonationContext: boolean;
    public isApproved: boolean;
    public data: Uint8Array;
    public identifierColumn: string;
    public createdBy?: string;
    public createdDate?: Date;
    public updatedBy?: string;
    public updatedDate?: Date;

    constructor(
        id: Guid,
        requesterEmail: string,
        recipientEmail: string,
        isImpersonationContext: boolean,
        isApproved: boolean,
        data: Uint8Array,
        identifierColumn: string,
        createdBy?: string,
        createdDate?: Date,
        updatedBy?: string,
        updatedDate?: Date,
    ) {
        this.id = id;
        this.requesterEmail = requesterEmail || "";
        this.recipientEmail = recipientEmail || "";
        this.isImpersonationContext = isImpersonationContext || false;
        this.isApproved = isApproved || false;
        this.data = data || 0;
        this.identifierColumn = identifierColumn || "";
        this.createdBy = createdBy !== undefined ? createdBy : '';
        this.createdDate = createdDate ;
        this.updatedBy = updatedBy !== undefined ? updatedBy : '';
        this.updatedDate = updatedDate;
    }
}