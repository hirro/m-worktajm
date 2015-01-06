//
//  BackendApiImpl.swift
//  worktajm
//
//  Created by Jim Arnell on 2015-01-06.
//  Copyright (c) 2015 Arnell Consulting AB. All rights reserved.
//

import Foundation
import SwiftyJSON
import Alamofire

class BackendApiImpl : BackendApi {
    let AuthUrl:String = "http://192.168.1.3:9000/auth/local"
    // let AuthUrl:String = "http://www.worktajm.com/auth/local"
    
    func login(username:String, password:String, completionHandler:String -> Void, errorHandler:[LoginResult] -> Void) {
        var errors = [LoginResult]()
        if (username.isEmpty) {
            errors.append(LoginResult.MissingUsername)
        }
        if (password.isEmpty) {
            errors.append(LoginResult.MissingPassword)
        }
        
        if (errors.isEmpty) {
            let parameters = [
                "email": username,
                "password": password
            ]
            Alamofire
                .request(.POST, AuthUrl, parameters: parameters, encoding: .JSON)
                .validate(statusCode: 200..<300)
                .validate(contentType: ["application/json"])
                .responseJSON { (_, response, JSON, error) in
                    let info = JSON as NSDictionary
                    var token:String? = info.valueForKey("token") as? String
                    var message:String? = info.valueForKey("message") as? String
                    if (token == nil) {
                        println("Login failed: \(message)")
                        errors.append(.InvalidCredentials)
                        errorHandler(errors)
                    } else {
                        println("Login successful: token\(token)")
                        completionHandler(token!)
                    }
            }
        } else {
            errorHandler(errors)
        }
    }
    
}
