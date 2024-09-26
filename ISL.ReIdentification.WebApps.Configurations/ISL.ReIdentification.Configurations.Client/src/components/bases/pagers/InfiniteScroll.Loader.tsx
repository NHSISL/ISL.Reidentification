import React, { FunctionComponent, ReactElement } from "react";

interface InfiniteScrollLoaderProps {
    children?: React.ReactNode;
    loading: boolean;
    spinner: ReactElement;
    noMorePages: boolean;
    noMorePagesMessage?: ReactElement;
    totalPages?: number;
}

const InfiniteScrollLoader: FunctionComponent<InfiniteScrollLoaderProps> = (props) => {
    const showNoMorePagesMessage = props.noMorePages && props.totalPages && props.totalPages > 1;

    return (
        <>
            {props.children}
            {props.loading && props.spinner}
            {showNoMorePagesMessage && props.noMorePagesMessage}
        </>
    )
}

export default InfiniteScrollLoader
