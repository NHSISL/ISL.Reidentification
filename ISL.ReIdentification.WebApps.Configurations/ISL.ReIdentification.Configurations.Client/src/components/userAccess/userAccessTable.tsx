import { debounce } from "lodash";
import React, { FunctionComponent, useMemo, useState } from "react";
import { SpinnerBase } from "../bases/spinner/SpinnerBase";
import { Card, Table } from "react-bootstrap";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faDatabase } from "@fortawesome/free-solid-svg-icons";
import { userAccessViewService } from "../../services/views/userAccess/lookupViewService";
import { UserAccessView } from "../../models/views/components/userAccess/userAccessView";
import UserAccessRow from "./userAccessRow";

type UserAccessTableProps = {};

const UserAccessTable: FunctionComponent<UserAccessTableProps> = (props) => {
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [debouncedTerm, setDebouncedTerm] = useState<string>("");
    const [showSpinner, setShowSpinner] = useState(false);


    const {
        mappedUserAccess: userAccessRetrieved,
        isLoading,
        fetchNextPage,
        isFetchingNextPage,
        hasNextPage,
        data,
        refetch
    } = userAccessViewService.useGetAllUserAccess(
        debouncedTerm
    );

    const handleSearchChange = (value: string) => {
        setSearchTerm(value);
        handleDebounce(value);
    };

    const handleDebounce = useMemo(
        () =>
            debounce((value: string) => {
                setDebouncedTerm(value);
            }, 500),
        []
    );

    const hasNoMorePages = () => {
        return !isLoading && data?.pages.at(-1)?.nextPage === undefined;
    };

    return (
        <div className="infiniteScrollContainer">
            <Card>
                <Card.Header> <FontAwesomeIcon icon={faDatabase} className="me-2" /> Ingestion Tracking</Card.Header>
                <Card.Body>
                    <InfiniteScroll loading={isLoading || showSpinner} hasNextPage={hasNextPage || false} loadMore={fetchNextPage}>

                        <Table>
                            <tbody>
                                {isLoading || showSpinner ? (
                                    <tr>
                                        <td colSpan={6} className="text-center">
                                            <SpinnerBase />
                                        </td>
                                    </tr>
                                ) : (
                                    <>
                                        {userAccessRetrieved?.map(
                                            (userAccessView: UserAccessView) => (
                                                    <UserAccessRow
                                                        key={userAccessView.id}
                                                        ingestionTracking={userAccessView}
                                                    />
                                                )
                                        )}
                                        <tr>
                                            <td colSpan={5} className="text-center">
                                                <InfiniteScrollLoader
                                                    loading={isLoading || isFetchingNextPage}
                                                    spinner={<SpinnerBase />}
                                                    noMorePages={hasNoMorePages()}
                                                    noMorePagesMessage={<>-- No more pages --</>}
                                                />
                                            </td>
                                        </tr>
                                    </>
                                )}
                            </tbody>
                        </Table>
                    </InfiniteScroll>
                </Card.Body>
            </Card>

        </div>
    );
};

export default UserAccessTable;