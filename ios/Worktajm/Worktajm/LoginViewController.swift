//
//  LoginViewController.swift
//  worktajm
//
//  Created by Jim Arnell on 2015-01-05.
//  Copyright (c) 2015 Arnell Consulting AB. All rights reserved.
//

import UIKit
import SwiftyJSON
import Alamofire

class LoginViewController: UIViewController {

    @IBOutlet weak var emailField: UITextField!
    @IBOutlet weak var passwordField: UITextField!
    
    let AuthUrl:String = "http://192.168.1.3:9000/auth/local"
    // let AuthUrl:String = "http://www.worktajm.com/auth/local"
    let SignupUrl:String = "http://worktajm.com/signup"
    
    override func viewDidLoad() {
        super.viewDidLoad()

        // Do any additional setup after loading the view.
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    @IBAction func onRegister(sender: AnyObject) {
        UIApplication.sharedApplication().openURL(NSURL(string: SignupUrl)!)
    }

    @IBAction func backgroundTap(sender: UIControl) {
        emailField.resignFirstResponder()
        passwordField.resignFirstResponder()
    }
    
    @IBAction func onLogin(sender: UIButton) {
        login(loginCompletionHandler, username: emailField.text, password: passwordField.text)
    }
    
    func login(completionHandler:String -> Void, username:String, password:String) -> [LoginResult] {
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
                        self.showLoginErrors(errors)
                    } else {
                        println("Login successful: token\(token)")
                        completionHandler(token!)
                    }
            }
        } else {
            showLoginErrors(errors)
        }
        
        return errors
    }
    
    private func loginCompletionHandler(token:String) {
        println("Logged in as \(token)")
        (self.parentViewController as RootViewController).switchViewToWorktajm()
    }
    
    private func showLoginErrors(loginResults:[LoginResult]) {
    
        if (loginResults.count > 0) {
            // Build error message
            var message = ""
            for loginResult in loginResults {
                switch (loginResult) {
                case .MissingPassword:
                    message += "Missing password"
                case .MissingUsername:
                    message += "Missing user name"
                case .InvalidCredentials:
                    message += "Invalid credentials"
                }
            }
            
            // Show alert
            let alert = UIAlertController(
                title: "Login failed",
                message: message,
                preferredStyle: .Alert)
            let cancelAction = UIAlertAction(title: "OK", style: .Default, handler: nil)
            alert.addAction(cancelAction)
            self.presentViewController(alert, animated: true, completion: nil)
            
        }
    }
    
}
