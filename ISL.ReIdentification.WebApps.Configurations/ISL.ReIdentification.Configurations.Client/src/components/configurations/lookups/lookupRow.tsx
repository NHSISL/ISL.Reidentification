import React, { FunctionComponent, useState } from "react";
import { LookupView } from "../../models/views/components/lookups/lookupView";
import LookupRowView from "./lookupRowView";

type LookupRowProps = {
    lookup: LookupView;
    allowedToEdit: boolean;
    allowedToDelete: boolean;
    onUpdate: (lookup: LookupView) => void;
    onDelete: (lookup: LookupView) => void;
}

const LookupRow: FunctionComponent<LookupRowProps> = (props) => {
    const {
        lookup,
        allowedToEdit,
        allowedToDelete,
        onUpdate,
        onDelete,
    } = props;

    const [mode, setMode] = useState<string>('VIEW');
    const [apiError, setApiError] = useState<any>({});


    const handleMode = (value: string) => {
        setMode(value);
    };

    //const handleUpdate = async (lookup: LookupView) => {
    //    try {
    //        await onUpdate(lookup);
    //        setMode('VIEW');
    //    } catch (error) {
    //        setApiError(error);
    //        setMode('EDIT');
    //    }
    //};

    //const handleDelete = (lookup: LookupView) => {
    //    onDelete(lookup);
    //    setMode('VIEW');
    //};

    //const handleCancel = () => {
    //    setMode('VIEW');
    //};

    return (
        <>
                <LookupRowView
                    key={lookup.id.toString()}
                    lookup={lookup}
                    onEdit={handleMode}
                    onDelete={handleMode}
                    allowedToEdit={allowedToEdit}
                    allowedToDelete={allowedToDelete} />
        </>
    );
}

LookupRow.defaultProps = {
    allowedToEdit: false,
    allowedToDelete: false
};

export default LookupRow;