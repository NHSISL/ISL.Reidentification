import React, { FunctionComponent, useMemo, useState } from "react";
import { debounce } from "lodash";

import SearchBase from "../bases/search/SearchBase";
import { SpinnerBase } from "../bases/spinner/SpinnerBase";
import { Card, CardBody, CardText, CardTitle, Container, Table } from "react-bootstrap";
import LookupRow from "./lookupRow";
import { LookupView } from "../../../models/views/components/lookups/lookupView";
import { lookupViewService } from "../../../services/views/lookups/lookupViewService";

type LookupTableProps = {
    allowedToAdd: boolean;
    allowedToEdit: boolean;
    allowedToDelete: boolean;
}

const LookupTable: FunctionComponent<LookupTableProps> = (props) => {
    const {
        allowedToAdd,
        allowedToEdit,
        allowedToDelete,
    } = props;

    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");

    const { mappedLookups: lookupsRetrieved, isLoading } = lookupViewService.useGetAllLookups(debouncedTerm);

    //const addLookup = lookupViewService.useCreateLookup();
    //const updateLookup = lookupViewService.useUpdateLookup();
    //const deleteLookup = lookupViewService.useRemoveLookup();

    const [addMode, setAddMode] = useState<boolean>(false);
    const [addApiError, setAddApiError] = useState<any>({});

    const handleSearchChange = (value: string) => {
        setSearchTerm(value);
        handleDebounce(value);
    }

    const handleAddState = () => {
        setAddMode(!addMode);
    };

    const handleAddNew = (lookup: LookupView) => {
        //return addLookup.mutate(lookup, {
        //    onSuccess: () => {
        //        setAddMode(false);
        //    },
        //    onError: (error: any) => {
        //        setAddApiError(error?.response?.data?.errors);
        //    }
        //});
    };

    const handleUpdate = (lookup: LookupView) => {
        //return updateLookup.mutateAsync(lookup);
    }

    const handleDelete = (lookup: LookupView) => {
        //return deleteLookup.mutateAsync(lookup.id);
    }

    const handleDebounce = useMemo(
        () => debounce((value: string) => {
            setDebouncedTerm(value)
        }, 500)
        , [])

    return (
        <>
            <Container fluid>
                <Card>
                    <Card.Header as="h5">
                        Lookup Configuration
                    </Card.Header>
                    <Card.Body>
                        <Table striped bordered hover variant="dark">
                            <thead>
                                <tr>
                                    <th>Name</th>
                                    <th>Value</th>
                                    <th>Action(s)</th>
                                </tr>
                            </thead>
                            <tbody>
                                {
                                    lookupsRetrieved?.map((lookup: LookupView) =>
                                        <LookupRow
                                            key={lookup.id?.toString()}
                                            lookup={lookup}
                                            allowedToEdit={allowedToEdit}
                                            allowedToDelete={allowedToDelete}
                                            onUpdate={handleUpdate}
                                            onDelete={handleDelete} />
                                    )}
                            </tbody>
                        </Table>
                    </Card.Body>
                </Card>
            </Container>
        </>
    );
}

export default LookupTable;