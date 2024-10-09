import { debounce } from "lodash";
import React, { FunctionComponent, useMemo, useState } from "react";
import { SpinnerBase } from "../bases/spinner/SpinnerBase";
import { Card, Form, FormGroup, Table } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faDatabase } from "@fortawesome/free-solid-svg-icons";
import { userAccessViewService } from "../../services/views/userAccess/lookupViewService";
import { UserAccessView } from "../../models/views/components/userAccess/userAccessView";
import UserAccessRow from "./userAccessRow";
import { entraUsersService } from "../../services/foundations/entraUsersService";

type EntraUserSearchProps = {};

const EntraUserSearch: FunctionComponent<EntraUserSearchProps> = (props) => {
    const [searchTerm, setSearchTerm] = useState<string>();
    const [debouncedTerm, setDebouncedTerm] = useState<string>();

    const { data } = entraUsersService.useSearchEntraUsers(debouncedTerm);

    const handleSearchChange = (value: string) => {
        setSearchTerm(value);
        handleDebounce(value);
    };

    const handleDebounce = useMemo(
        () =>
            debounce((value: string) => {
                if(value.length) {
                    setDebouncedTerm(value);
                }
            }, 500),
        []
    );



    return (
        <Card>
            <Card.Header> <FontAwesomeIcon icon={faDatabase} className="me-2" /> Ingestion Tracking</Card.Header>
            <Card.Body>
                <Form>
                    <FormGroup>
                        <Form.Label>Email Address</Form.Label>
                        <Form.Control autoComplete="off" type="text" placeholder="Enter email address" onChange={(e) => handleSearchChange(e.target.value)} value={searchTerm}/>
                    </FormGroup>
                    <div>SEARCH TERM: {debouncedTerm}</div>
                    <div>
                        {data && data.map((user: any) => ( <div>{user.mail}</div>))}

                    </div>
                </Form>
            </Card.Body>
        </Card>
    );
};

export default EntraUserSearch;