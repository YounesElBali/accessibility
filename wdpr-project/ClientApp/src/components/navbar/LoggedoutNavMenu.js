import {NavItem, NavLink} from "reactstrap";
import {Link} from "react-router-dom";
import React from "react";

const LoggedoutNavMenu = () => {
    return (<>
        <NavItem>
            <NavLink tag={Link} className="text-light" to="/login">
                Login
            </NavLink>
        </NavItem>
        <NavItem>
            <NavLink tag={Link} className="text-light" to="/register/select">
                Registreer
            </NavLink>
        </NavItem> 
    </>)
}

export default LoggedoutNavMenu;