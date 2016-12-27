import React, {Component} from 'react'

class Login extends Component {
    render () {
        return (
            <div>
                Enter you name:
                <input type="text" />
                <input type="submit" />
            </div>
        )
    }
}

export default Login