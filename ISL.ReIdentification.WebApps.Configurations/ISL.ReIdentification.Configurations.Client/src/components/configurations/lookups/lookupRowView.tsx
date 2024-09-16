import React, { FunctionComponent } from "react";
import { LookupView } from "../../models/views/components/lookups/lookupView";

interface LookupRowViewProps {
    lookup: LookupView;
    allowedToEdit: boolean;
    allowedToDelete: boolean;
    onEdit: (value: string) => void;
    onDelete: (value: string) => void;
}

const LookupRowView: FunctionComponent<LookupRowViewProps> = (props) => {
    const {
        lookup,
        allowedToEdit,
        allowedToDelete,
        onEdit,
        onDelete
    } = props;

    return (
        <tr>
            <td>
                dsfds

            </td>
            <td>
                dsfdsf
            </td>
            <td>
                dsfds
            </td>
        </tr>
    );
}

export default LookupRowView;
