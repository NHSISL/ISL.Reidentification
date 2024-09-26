import React, { FunctionComponent, ChangeEvent, useState, useEffect } from "react";
import { LookupView } from "../../../models/views/components/lookups/lookupView";
import { lookupErrors } from "./lookupErrors";
import { lookupValidations } from "./lookupValidations";
import TextInputBase from "../../bases/inputs/TextInputBase";
import { Button } from "react-bootstrap";
import { useValidation } from "../../../hooks/useValidation";

interface LookupRowEditProps {
    lookup: LookupView;
    onCancel: () => void;
    onEdit: (supplier: LookupView) => void;
    apiError?: any;
}

const LookupRowEdit: FunctionComponent<LookupRowEditProps> = (props) => {
    const {
        lookup,
        onCancel,
        onEdit,
        apiError
    } = props;

    const [editLookup, setEditLookup] = useState<LookupView>({ ...lookup });

    const { errors, processApiErrors, enableValidationMessages, validate } =
        useValidation(lookupErrors, lookupValidations, editLookup);

    const handleChange = (event: ChangeEvent<HTMLInputElement> | ChangeEvent<HTMLTextAreaElement>) => {
        const updatedLookup = {
            ...editLookup,
            [event.target.name]: event.target.type === "checkbox"
                ? (event.target as HTMLInputElement).checked : event.target.value,
        };

        setEditLookup(updatedLookup);
    };

    const handleCancel = () => {
        setEditLookup({ ...lookup });
        onCancel();
    };

    const handleUpdate = () => {
        if (!validate(editLookup)) {
            onEdit(editLookup);
        } else {
            enableValidationMessages();
        }
    }

    useEffect(() => {
        processApiErrors(apiError);
    }, [apiError, processApiErrors])

    return (
        <tr>
            <td>

                <TextInputBase
                    id="name"
                    name="name"
                    label="Lookup Name"
                    placeholder="Lookup Name"
                    value={editLookup.name}
                    required={true}
                    error={errors.name}
                    onChange={handleChange} />

                <br />

                <TextInputBase
                    id="value"
                    name="value"
                    label="Lookup Value"
                    placeholder="Lookup Value"
                    value={editLookup.value}
                    required={true}
                    error={errors.name}
                    onChange={handleChange} />

            </td>

            <br />

            <td className="text-end">
                <Button onClick={() => handleCancel()}>Cancel</Button>&nbsp;
                <Button onClick={() => handleUpdate()} disabled={errors.hasErrors} >Update</Button>
            </td>
        </tr>
    );
}

export default LookupRowEdit;