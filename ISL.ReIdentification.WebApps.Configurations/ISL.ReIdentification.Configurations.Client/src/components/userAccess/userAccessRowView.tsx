import React, { FunctionComponent } from "react";
import { Link } from 'react-router-dom';
import { UserAccess } from "../../models/userAccess/userAccess";
import { Button } from "react-bootstrap";

type UserAccessRowProps = {
    userAccess: UserAccess;
}

const UserAccessRow: FunctionComponent<UserAccessRowProps> = (props) => {
    const {
        userAccess
    } = props;


    return (
        <tr>
            <td>
                <div className="p-2 rounded al text-center">
                    {userAccess.recipientEmail}
                </div>
            </td>

            <td>
                <Link to={`/ingestionTrackingDetail/${userAccess.id}`}>
                    <Button onClick={() => { }}>
                        Details
                    </Button>
                </Link>
            </td>

        </tr>
    );
}

export default UserAccessRow;