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
      
        var parent:RootViewController = self.parentViewController as RootViewController
        parent.login(emailField.text, password: passwordField.text, completionHandler: loginCompletionHandler, errorHandler: showLoginErrors)
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
                case .FailedToConnect:
                  message += "Failed to connect"
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
