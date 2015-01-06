//
//  BackendAPI.swift
//  worktajm
//
//  Created by Jim Arnell on 2015-01-06.
//  Copyright (c) 2015 Arnell Consulting AB. All rights reserved.
//

import Foundation

protocol BackendApi {

    /**
    Login to the backend
    
    :param: username The user name
    :param: password The password
    :param completionHandler Completion handler, called with the token id on success
    :param errorHandler Error handler, called with a list of login errors.
    :returns: Void
    */
    func login(username:String, password:String, completionHandler:String->Void, errorHandler:[LoginResult]->Void)
    
}
